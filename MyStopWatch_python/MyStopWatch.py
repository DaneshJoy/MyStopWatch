import sys
import json
from PyQt5.QtWidgets import QApplication, QWidget, QVBoxLayout, QPushButton, QLabel, QHBoxLayout
from PyQt5.QtCore import QTimer, QTime, Qt, QPoint
from PyQt5.QtGui import QPalette, QColor


class Stopwatch(QWidget):
    def __init__(self):
        super().__init__()

        self.initUI()

        self.timer = QTimer(self)
        self.timer.timeout.connect(self.updateTime)

        self.elapsed_time = QTime(0, 0)
        self.loadElapsedTime()
        self.running = False

        self.save_timer = QTimer(self)
        self.save_timer.timeout.connect(self.saveElapsedTime)
        self.save_timer.start(30000)  # Save every 30 seconds

        self.drag_position = None

    def initUI(self):
        self.setWindowTitle('Digital Stopwatch')

        self.setWindowFlags(Qt.FramelessWindowHint)
        self.setAttribute(Qt.WA_TranslucentBackground)
        self.setAutoFillBackground(True)

        self.setStyleSheet("""
            background-color: rgba(0, 0, 0, 127);
            border: 2px solid rgba(255, 255, 255, 255);
            border-radius: 15px;
        """)

        self.layout = QVBoxLayout()
        self.setLayout(self.layout)

        self.top_bar = QHBoxLayout()
        self.layout.addLayout(self.top_bar)

        self.top_bar.addStretch()

        self.minimize_button = QPushButton('᠆', self)
        self.minimize_button.setStyleSheet(
            "background-color: rgba(80,100,100,200); color: white; font-size: 20px; border: none; border-radius: 5px;")
        self.minimize_button.setFixedSize(20, 20)
        self.minimize_button.clicked.connect(self.showMinimized)
        self.top_bar.addWidget(self.minimize_button)
        cursor = Qt.PointingHandCursor
        self.setCursor(cursor)

        self.close_button = QPushButton('X', self)
        self.close_button.setStyleSheet(
            "background-color: rgba(80,100,100,200); color: white; font-size: 18px; border: none; border-radius: 5px;")
        self.close_button.setFixedSize(20, 20)
        self.close_button.clicked.connect(self.close)
        self.top_bar.addWidget(self.close_button)

        self.time_label = QLabel('00:00:00', self)
        self.time_label.setStyleSheet("font-size: 30px; color: white;")
        self.time_label.setAlignment(Qt.AlignCenter)
        self.layout.addWidget(self.time_label)

        self.buttons = QHBoxLayout()
        self.layout.addLayout(self.buttons)
        self.start_pause_button = QPushButton('▶', self)
        self.start_pause_button.setStyleSheet(
            "background-color: rgba(40,170,70,100); color: white; font-size: 20px; border-radius: 10px;")
        self.start_pause_button.clicked.connect(self.startPause)
        self.buttons.addWidget(self.start_pause_button)

        self.reset_button = QPushButton('■', self)
        self.reset_button.setStyleSheet(
            "background-color: rgba(220,50,70,100); color: white; font-size: 20px; border-radius: 10px;")
        self.reset_button.clicked.connect(self.reset)
        self.buttons.addWidget(self.reset_button)

        self.layout.setContentsMargins(5, 5, 5, 5)
        self.setWindowPosition()

    def setWindowPosition(self):
        screen_geometry = QApplication.primaryScreen().geometry()
        screen_width = screen_geometry.width()
        screen_height = screen_geometry.height()
        # self_width = self.width()
        # self_height = self.height()
        taskbar_height = 40  # Approximate height of the taskbar
        # self.move(screen_width - self_width - 10, screen_height -
        #           self_height - taskbar_height - 10)
        self.move(screen_width - 10, screen_height - taskbar_height - 10)

    def startPause(self):
        if self.running:
            self.timer.stop()
            self.running = False
            self.start_pause_button.setText('▶')
        else:
            self.timer.start(1000)
            self.running = True
            self.start_pause_button.setText('꤯꤯')

    def reset(self):
        self.timer.stop()
        self.running = False
        self.elapsed_time = QTime(0, 0)
        self.time_label.setText(self.elapsed_time.toString('hh:mm:ss'))
        self.start_pause_button.setText('▶')
        self.saveElapsedTime()

    def updateTime(self):
        self.elapsed_time = self.elapsed_time.addSecs(1)
        self.time_label.setText(self.elapsed_time.toString('hh:mm:ss'))

    def saveElapsedTime(self):
        with open('elapsed_time.json', 'w') as file:
            json.dump(
                {'elapsed_time': self.elapsed_time.toString('hh:mm:ss')}, file)

    def loadElapsedTime(self):
        try:
            with open('elapsed_time.json', 'r') as file:
                data = json.load(file)
                self.elapsed_time = QTime.fromString(
                    data['elapsed_time'], 'hh:mm:ss')
                self.time_label.setText(self.elapsed_time.toString('hh:mm:ss'))
        except FileNotFoundError:
            pass

    def mousePressEvent(self, event):
        if event.button() == Qt.LeftButton:
            self.drag_position = event.globalPos() - self.frameGeometry().topLeft()
            event.accept()

    def mouseMoveEvent(self, event):
        if event.buttons() == Qt.LeftButton:
            self.move(event.globalPos() - self.drag_position)
            event.accept()


if __name__ == '__main__':
    app = QApplication(sys.argv)
    stopwatch = Stopwatch()
    stopwatch.show()
    sys.exit(app.exec())
